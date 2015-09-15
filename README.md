# TumblrUniversal
An library that works with the Windows Universal Apps Platfrom to deliver Tumblr APIs for your projects.

# Current Use

``` git submodule add https://github.com/KarnSaheb/TumblrUniversal.git ```

Then manually add the project to your UAP solution.

# Dependencies

[SQLite for the Universal Windows Platform](https://visualstudiogallery.msdn.microsoft.com/4913e7d5-96c9-4dde-a1a1-69820d615936)


# Authenticating

## xAuth

Create a new instance of the Client object with the API and API secret keys:

```var client = new TumblrUniversal.Tumblr.TumblrClient("API_KEY", "API_SECRET_KEY");```

Send username and password:

```await client.Authentication.RequestAccessToken("USERNAME", "PASSWORD");```

Validate sign in if no exceptions are thrown:

```if (client.SignedIn)```


