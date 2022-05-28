using Microsoft.JSInterop;

namespace IndexedDbStorage.Utils;

public class IndexedDbAccessor
{
    private Lazy<IJSObjectReference> _accessorJsRef = new();
    private readonly IJSRuntime _jsRuntime;

    public IndexedDbAccessor(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("initialize");
    }

    public async Task<T> GetValueAsync<T>(string collectionName, int id)
    {
        await WaitForReference();
        var result = await _accessorJsRef.Value.InvokeAsync<T>("get", collectionName, id);

        return result;
    }

    public async Task SetValueAsync<T>(string collectionName, T value)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("set", collectionName, value);
    }

    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false)
        {
            _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/IndexedDbAccessor.js"));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_accessorJsRef.IsValueCreated)
        {
            await _accessorJsRef.Value.DisposeAsync();
        }
    }
}