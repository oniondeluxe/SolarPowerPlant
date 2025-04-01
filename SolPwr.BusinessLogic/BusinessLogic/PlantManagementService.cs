using Microsoft.Extensions.Logging;
using OnionDlx.SolPwr.BusinessObjects;
using OnionDlx.SolPwr.Data;
using OnionDlx.SolPwr.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.BusinessLogic
{
    internal class PlantManagementService : IPlantManagementService
    {
        readonly IUtilitiesRepositoryFactory _repoFac;
        readonly ILogger<IPlantManagementService> _logger;
        readonly IMeteoLookupServiceCallback _meteoCallback;
        readonly UnitOfWork<IUtilitiesRepository, Dto.PlantMgmtResponse> _agent;

        private UnitOfWork<IUtilitiesRepository, Dto.PlantMgmtResponse> Agent
        {
            get
            {
                return _agent;
            }
        }

        private IUtilitiesRepositoryFactory Database
        {
            get
            {
                return _repoFac;
            }
        }


        public async Task<Dto.PlantMgmtResponse> CreatePlantAsync(Dto.PowerPlant dtoRegister)
        {
            var newId = Guid.NewGuid();
            var resultDto = await Agent.ExecuteCommandAsync((repo, cmd) =>
            {
                var plant = new PowerPlant
                {
                    Id = newId,
                    UtcInstallDate = dtoRegister.UtcInstallDate,
                    PlantName = dtoRegister.PlantName,
                    PowerCapacity = dtoRegister.PowerCapacity,
                    Location = dtoRegister.Location
                };

                repo.PowerPlants.Add(plant);
                return cmd.AsSuccessful(true);
            },
            (dto) =>
            {
                // The payload will carry the newly created ID
                dto.Id = newId;
            });

            // Make sure we start feeding power data in the worker thread
            await _meteoCallback.GetEndpoint()?.StartFeedAsync(resultDto.Id.Value, dtoRegister.Location);

            return resultDto;
        }


        public async Task<Dto.PlantMgmtResponse> UpdatePlantAsync(Guid identity, Dto.PowerPlant dtoRegister)
        {
            return await Agent.ExecuteCommandAsync((repo, cmd) =>
            {
                var target = from plant in repo.PowerPlants where plant.Id == identity select plant;
                if (!target.Any())
                {
                    // From an API/PUT perspective, this is a success (idempotent operation)
                    return cmd.AsSuccessful($"Plant '{identity}' not found");
                }

                var modify = target.First();
                modify.UtcInstallDate = dtoRegister.UtcInstallDate;
                modify.PlantName = dtoRegister.PlantName;
                modify.PowerCapacity = dtoRegister.PowerCapacity;
                modify.Location = dtoRegister.Location;

                return cmd.AsSuccessful(true);
            });
        }


        public async Task<Dto.PlantMgmtResponse> DeletePlantAsync(Guid identity)
        {
            return await Agent.ExecuteCommandAsync((repo, cmd) =>
            {
                var target = from plant in repo.PowerPlants where plant.Id == identity select plant;
                if (!target.Any())
                {
                    // From an API/PUT perspective, this is a success (idempotent operation)
                    return cmd.AsSuccessful($"Plant '{identity}' not found");
                }

                var modify = target.First();
                var history = from rec in repo.GenerationRecords where rec.PowerPlant.Id == identity select rec;
                repo.GenerationRecords.RemoveRange(history);
                repo.PowerPlants.Remove(modify);

                return cmd.AsSuccessful(true);
            });
        }


        public async Task<IEnumerable<Dto.PowerPlantImmutable>> GetAllPlantsAsync()
        {
            var result = new List<Dto.PowerPlantImmutable>();
            using (var repo = Database.NewQuery())
            {
                await foreach (var dbRecord in repo.PowerPlants)
                {
                    result.Add(dbRecord.ToDto());
                }
            }

            return result;
        }


        private void OnUpdateProductionData(IMeteoLookupService meteo)
        {
            var now = DateTime.UtcNow;
            var tempStorage = new List<(Guid, IList<Dto.MeteoData>)>();
            var res = Agent.ExecuteQueryAsync<Dto.MeteoData>(async (context) =>
            {
                // Go through all plants and find their location etc
                var plants = from p in context.PowerPlants select p;
                foreach (var plant in plants)
                {
                    var mdList = new List<Dto.MeteoData>();
                    // Zero values means right now
                    var data = await meteo.GetMeteoDataAsync(plant.Location, TimeResolution.None, TimeSpanCode.None, 0);
                    mdList.AddRange(data);
                    tempStorage.Add((plant.Id, mdList));
                }

                return null;
            }).Result;

            // Now, update the DB with these added rows
            foreach (var item in tempStorage)
            {
                // TODO: Insert rows
            }
        }


        public async Task<Dto.PlantMgmtResponse> SeedPlantsAsync(int quartersBehind)
        {
            // We only go back in time
            if (quartersBehind <= 0)
            {
                return Dto.PlantMgmtResponse.CreateFaulted("Invalid seed value");
            }

            return await Agent.ExecuteCommandAsync((repo, cmd) =>
            {
                var counter = 0;
                var now = DateTime.UtcNow;
                foreach (var plant in repo.PowerPlants)
                {
                    for (int i = quartersBehind; i > 0; i--)
                    {
                        var totalMinutes = 15 * i;
                        var historyRec = new PowerGenerationRecord
                        {
                            Id = Guid.NewGuid(),
                            PowerPlant = plant,
                            UtcTimestamp = now.AddMinutes(-totalMinutes),
                            PowerGenerated = Random.Shared.Next(0, 100) * plant.PowerCapacity / 100.0
                        };

                        repo.GenerationRecords.Add(historyRec);
                    }
                    counter++;
                }

                return cmd.AsSuccessful(counter.ToString(), true);
            });
        }


        public async Task<IEnumerable<Dto.PlantPowerData>> GetPowerDataAsync(Guid identity, PowerDataTypes type, TimeResolution resol, TimeSpanCode code, int timeSpan)
        {
            // History: we read from the stored data
            if (type == PowerDataTypes.History)
            {
                return await Agent.ExecuteQueryAsync<Dto.PlantPowerData>(async (context) =>
                {
                    var plants = from p in context.PowerPlants
                                 where p.Id == identity
                                 select p;
                    var currentPlant = await plants.FirstOrDefaultAsync();
                    if (currentPlant != null)
                    {
                        var result = new List<Dto.PlantPowerData>();
                        var history = from hist in context.GenerationRecords
                                      where hist.PowerPlant.Id == currentPlant.Id
                                      select hist;
                        foreach (var histItem in await history.ToListAsync())
                        {
                            result.Add(new Dto.PlantPowerData
                            {
                                PlantId = currentPlant.Id,
                                CurrentPower = histItem.PowerGenerated,
                                UtcTime = histItem.UtcTimestamp
                            });
                        }

                        return result;
                    }
                    else
                    {
                        return Array.Empty<Dto.PlantPowerData>();
                    }
                });
            }
            else if (type == PowerDataTypes.Forecast)
            {
                // Forecast, we ask the meteo service
                var meteo = _meteoCallback.GetEndpoint();

                return await Agent.ExecuteQueryAsync<Dto.PlantPowerData>(async (context) =>
                {
                    var plants = from p in context.PowerPlants
                                 where p.Id == identity
                                 select p;
                    var currentPlant = await plants.FirstOrDefaultAsync();
                    if (currentPlant != null)
                    {
                        var result = new List<Dto.PlantPowerData>();

                        // Fake science - The latitude will influence how much power the sun will generate
                        var now = DateTime.UtcNow;
                        var data = await meteo.GetMeteoDataAsync(currentPlant.Location, resol, code, timeSpan);
                        foreach (var dp in data)
                        {
                            var calc = new PowerCalculator(currentPlant.PowerCapacity, currentPlant.Location.Latitude);
                            var power = calc.GetCurrentPower(dp.WeatherCode, dp.Visibility);
                            result.Add(new Dto.PlantPowerData
                            {
                                PlantId = currentPlant.Id,
                                CurrentPower = power,
                                UtcTime = dp.UtcTime
                            });
                        }

                        return result;
                    }
                    else
                    {
                        return Array.Empty<Dto.PlantPowerData>();
                    }
                });
            }

            // Nothing ordered, or nothing found
            return Array.Empty<Dto.PlantPowerData>();
        }


        public PlantManagementService(IUtilitiesRepositoryFactory repo, ILogger<IPlantManagementService> logger, IMeteoLookupServiceCallback factory)
        {
            _logger = logger;
            _repoFac = repo;

            // Setup the connection between the Persistence contracts and our Dto contracts, 
            // with logging as an aspect
            var dtoFactory = new Dto.DtoFactory<Dto.PlantMgmtResponse>((guid) =>
            {
                return new Dto.PlantMgmtResponse { TransactionId = guid };
            });
            _agent = _repoFac.CreateUoW(_logger).For(dtoFactory);

            // Subscribe to events coming from the outer worker
            _meteoCallback = factory;
            _meteoCallback.ServicePushUpdate += (sender, service) =>
            {
                // The background worker will invoke this, but WE have to take care if it, using the 
                // caller, to ask for data from the Internet
                OnUpdateProductionData(service);
            };
        }
    }
}
