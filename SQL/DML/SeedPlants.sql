DELETE FROM spa.GenerationHistory
DELETE FROM spa.PowerPlants

INSERT INTO spa.PowerPlants
(Id, PlantName, UtcInstallDate, Latitude, Longitude, PowerCapacity)
VALUES
('0720072C-1A86-4113-B460-0C0AAA406C6A', 'Lower Manhattan', '2001-09-11T08:46:00', 38.8, 12.7, 6000)


INSERT INTO spa.PowerPlants
(Id, PlantName, UtcInstallDate, Latitude, Longitude, PowerCapacity)
VALUES
('9C8D7E47-5F83-4BE8-B323-D9D2F1439FD0', 'Mare Tranquilitatis', '1969-07-20T20:17:40', 89.4, 2.31, 2000)

