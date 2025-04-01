using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.QualityAssurance
{
    internal class AsyncMockCollection<TElement> : IEnumerable<TElement>, IAsyncEnumerable<TElement>
    {
        readonly List<TElement> _source;

        public AsyncMockCollection(IEnumerable<TElement> source)
        {
            _source = new List<TElement>(source);
        }


        public IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var enumerator = _source.GetEnumerator();
            return new AsyncEnumeratorWrapper<TElement>(enumerator);
        }


        public IEnumerator<TElement> GetEnumerator()
        {
            return _source.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private class AsyncEnumeratorWrapper<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public AsyncEnumeratorWrapper(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public ValueTask DisposeAsync()
            {
                _inner.Dispose();
                return ValueTask.CompletedTask;
            }

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(_inner.MoveNext());
            }

            public T Current => _inner.Current;
        }

    }
}
