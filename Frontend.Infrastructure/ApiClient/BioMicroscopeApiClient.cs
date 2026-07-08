using System.Net.Http.Json;

namespace UCR.EB.BioMicroscopeAdmin.Frontend.Infrastructure.ApiClient;

/// <summary>
/// Typed HTTP client that wraps <see cref="HttpClient"/> using System.Net.Http.Json.
/// Follows the Kiota-style request-adapter pattern; can be replaced by a generated
/// client (see kiota-config.json) once the backend exposes its OpenAPI document.
/// </summary>
internal sealed class BioMicroscopeApiClient(HttpClient http)
{
    private static async Task<T> ReadCreatedAsync<T>(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<T>();
        return result ?? throw new InvalidOperationException("Empty response body from API.");
    }

    // Groups
    public Task<List<GroupResponse>?> GetGroupsAsync() =>
        http.GetFromJsonAsync<List<GroupResponse>>("groups");

    public Task<GroupResponse?> GetGroupAsync(int id) =>
        http.GetFromJsonAsync<GroupResponse>($"groups/{id}");

    public async Task<GroupResponse> CreateGroupAsync(CreateGroupRequest body) =>
        await ReadCreatedAsync<GroupResponse>(await http.PostAsJsonAsync("groups", body));

    public async Task<GroupResponse> UpdateGroupAsync(int id, CreateGroupRequest body) =>
        await ReadCreatedAsync<GroupResponse>(await http.PutAsJsonAsync($"groups/{id}", body));

    public async Task DeleteGroupAsync(int id) =>
        (await http.DeleteAsync($"groups/{id}")).EnsureSuccessStatusCode();

    // Boxes
    public Task<List<BoxResponse>?> GetBoxesAsync() =>
        http.GetFromJsonAsync<List<BoxResponse>>("boxes");

    public Task<BoxResponse?> GetBoxAsync(int id) =>
        http.GetFromJsonAsync<BoxResponse>($"boxes/{id}");

    public Task<List<BoxResponse>?> GetBoxesByGroupAsync(int groupId) =>
        http.GetFromJsonAsync<List<BoxResponse>>($"groups/{groupId}/boxes");

    public async Task<BoxResponse> CreateBoxAsync(CreateBoxRequest body) =>
        await ReadCreatedAsync<BoxResponse>(await http.PostAsJsonAsync("boxes", body));

    public async Task<BoxResponse> UpdateBoxAsync(int id, UpdateBoxRequest body) =>
        await ReadCreatedAsync<BoxResponse>(await http.PutAsJsonAsync($"boxes/{id}", body));

    public async Task DeleteBoxAsync(int id) =>
        (await http.DeleteAsync($"boxes/{id}")).EnsureSuccessStatusCode();

    // Slides
    public Task<PagedSlidesResponse?> GetSlidesAsync(string? search, int? boxId, int page, int pageSize)
    {
        var query = new List<string> { $"page={page}", $"pageSize={pageSize}" };
        if (!string.IsNullOrWhiteSpace(search)) query.Add($"search={Uri.EscapeDataString(search)}");
        if (boxId is not null) query.Add($"boxId={boxId}");
        return http.GetFromJsonAsync<PagedSlidesResponse>($"slides?{string.Join('&', query)}");
    }

    public Task<SlideDetailResponse?> GetSlideAsync(int id) =>
        http.GetFromJsonAsync<SlideDetailResponse>($"slides/{id}");

    public Task<List<SlideResponse>?> GetSlidesByBoxAsync(int boxId) =>
        http.GetFromJsonAsync<List<SlideResponse>>($"boxes/{boxId}/slides");

    public async Task<SlideResponse> CreateSlideAsync(CreateSlideRequest body) =>
        await ReadCreatedAsync<SlideResponse>(await http.PostAsJsonAsync("slides", body));

    public async Task<SlideResponse> UpdateSlideAsync(int id, UpdateSlideRequest body) =>
        await ReadCreatedAsync<SlideResponse>(await http.PutAsJsonAsync($"slides/{id}", body));

    public async Task DeleteSlideAsync(int id) =>
        (await http.DeleteAsync($"slides/{id}")).EnsureSuccessStatusCode();

    // Images
    public Task<List<ImageResponse>?> GetImagesAsync(int slideId) =>
        http.GetFromJsonAsync<List<ImageResponse>>($"slides/{slideId}/images");

    public async Task<ImageResponse> UploadImageAsync(int slideId, Stream fileStream, string fileName, int order)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(fileStream);
        content.Add(fileContent, "file", fileName);
        content.Add(new StringContent(order.ToString()), "order");
        return await ReadCreatedAsync<ImageResponse>(
            await http.PostAsync($"slides/{slideId}/images", content));
    }

    public async Task<ImageResponse> UpdateImageOrderAsync(int id, UpdateImageOrderRequest body) =>
        await ReadCreatedAsync<ImageResponse>(await http.PutAsJsonAsync($"images/{id}", body));

    public async Task DeleteImageAsync(int id) =>
        (await http.DeleteAsync($"images/{id}")).EnsureSuccessStatusCode();
}
