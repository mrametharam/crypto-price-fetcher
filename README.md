# crypto-price-fetcher
A .net core solution that pulls crypto symbols and their prices and saves them.

##Phases##

* 1: Build a console application that will make an API call to get a list of crypto symbols then make another call for each crypto, to get the price.
* 2: Build a web API that has an endpoint that returns the list of crypto and another that takes in a crypto and returns the price.
* 3: Clean things up by implementing the Clean Architecture.
* 4: Build a front-end web page (HTML, CSS, JS) that consumes the API endpoints.
* 5: Build a backend worker that fetches the list of crypto symbols and publishes it.
     Build another backend worker that subscribes to that event and takes the data and makes an API call to get the price of each crypto and publishes the results.
	 Update the web app to subscribe to the event and refreshes the data on the page.
* 6: Upgrade the web app to a blazor app.
