---
sidebar_label: 'Examples'
sidebar_position: 2
---

# Examples

### Post
```csharp
IThreadsApi api = new ThreadsApi(new HttpClient());
var authToken = await api.GetTokenAsync("tidusjar", "password");
await api.PostAsync("tidusjar", "Hello!", authToken);
```

### Follow & UnFollow
```csharp
IThreadsApi api = new ThreadsApi(new HttpClient());

var authToken = await api.GetTokenAsync("tidusjar", "password");

var userNameToFollow = "zuck";
var userToFollow = await api.GetUserIdFromUserNameAsync(userNameToFollow);
await api.FollowAsync(userToFollow.UserId, userToFollow.Token, authToken);
await api.UnFollowAsync(userToFollow.UserId, userToFollow.Token, authToken);
```