# Fangs Builder backend api  
Runs on azure VM, GoDaddy DNS A record pointing to VM: api.fangsbuilder.com  


# Features  
CRUD for models(Heroes, Augments, Artifacts,etc).  
OAuth2(authorization code grant flow) using discord roles from the Fangs server to determine what youre allowed to do. No user data persistence at the moment, 
the data is used to generate a JWT stored on the client as an httponly cookie that keeps all the claims needed.



## Oauth2 Flow Explanation
What the hell is Oauth2? Heres a good explanation in plain english(by Nate Barbetini): https://www.youtube.com/watch?v=996OiexHze0  
Heres how the flow looks for FangsBuilder in particular("ANGULAR UI" represents: https://github.com/nichitacebotari0/FangsBuilder-ng ).  
User only directly interacts with the ANGULAR UI.
![image](https://user-images.githubusercontent.com/37653982/206917155-e305c16e-3cd9-41a6-81f7-d061a5d6e501.png)

1) User clicks discord icon in angular which points to specific link to discord that includes the client_id of the app, the callback link(back to angular UI), 
and the scopes required("identify" and "guild.members.read", to find out what roles user has). This redirects user to page asking for authorization.
2) If the user authorizes, the response contains an authorization code.
3) Angular UI uses the authorization code to and makes a request to the api's "Login" endpoint .
4) Web api uses the access code, the client_id, and a client_secret only known to it(provided together with client_id by discord when registering an app) and makes a request to discord authorisation endpoint
5) Discord checks the info and respons with an access_token. This is the token we can use to access the scopes we asked for beore(in our case:identify and guild.members.read).
6) We make 2 requests to Discord's API passing in the access token, first asking for identify scoped data(username for ex.), next asking for Fangs Server membership info
7) Discord validates the token and gives us a partial user object(no email, as the scope wasnt included in the initial #1 request) for the first request, and then giving us info about user in the Fangs server, were specifically interested in roles.
8) Api checks the role ids for known ones(moderator, developer, is a region assigned) and generates a number of claims interesting to us.
Then using those claims a JWT token is generated in the API, signed by a secret key known only to the API and sent back to the ANGULAR UI as an httponly cookie.
Api also sends some non httponly cookies with some of the claims so angular can make decisions on what to show, but those dont affect actual api authorisation.
All subsequent requests to the api have the JWT cookie attached and its usied to authorise endpoint by policy(e.g. moderator and developer people can make edits)

Note: This JWT is set to expire within 6 days, and there is no user persistence in the webapi itself at all. Meaning api saves NO data about the user at the moment, this means that once the JWT expires we do not send a refresh request to the discord api(step #5 returns a refresh_token as well), we simply ask the user to reauthorise, meaning we repeat the flow from the start. If we do not want the user to authorise every single time, need to save the refresh_token somewhere, and also control access to the refreshing once JWT is expired.
