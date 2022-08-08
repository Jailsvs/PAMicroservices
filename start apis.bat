start /min cmd /k "dotnet run --project ./AuctionMicroservice/AuctionMicroservice.csproj"
start /min cmd /k "dotnet run --project ./UserMicroservice/UserMicroservice.csproj"
start /min cmd /k "dotnet run --project ./CompanyMicroservice/CompanyMicroservice.csproj"
timeout 3 > NUL
start /min cmd /k "dotnet run --project ./StopwatchMicroservice/StopwatchMicroservice.csproj"
start /min cmd /k "npm start --prefix ../PAReact"
timeout 5 > NUL
start /min cmd /k cd "c:\Program Files\Google\Chrome\Application\" & start chrome.exe -incognito http://localhost:3000/"  
start /min cmd /k cd "c:\Program Files\Google\Chrome\Application\" & start chrome.exe -incognito https://localhost:5001/swagger/index.html"  
start /min cmd /k cd "c:\Program Files\Google\Chrome\Application\" & start chrome.exe -incognito https://localhost:5003/swagger/index.html"  
start /min cmd /k cd "c:\Program Files\Google\Chrome\Application\" & start chrome.exe -incognito https://localhost:5005/swagger/index.html"  
start /min cmd /k cd "c:\Program Files\Google\Chrome\Application\" & start chrome.exe -incognito https://localhost:5007/swagger/index.html"  
 
 