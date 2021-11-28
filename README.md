# --------------------------------------------------- Comments, 28th Nov. 2021 ------------------------------------------------------------------------------

The application has most of the requirements implemented, but the implementation is not 100% as I did not feel everything is necessary. I am adding comments on each projects
to explain why I made certain choices and what is missing or was not important to implement. The code itself does not have any comments added, except the Authorize attribute
and the authentication setup in the StartUp.cs file to explain what should be done there.

I renamed certain projects to give a better understandability of which is doing what, so the naming is somewhat different to the original project.

## Core

This project works like a business layer, where we have the logic implemented. It has the interfaces that any dataaccesses need to implement, it has the custom exceptions
that the clients need to catch and the url providers that are used to generate action links.
The entities in this project are "typed" entities. It means where we use a date field its type is DateTime, where we use numbers the fields are the corresponding numeric field, etc.

I only implemented the DeliveryService, because an OrderService to further manage Orders would be quite similar and would not give any additional value to the result, thus not important.
The DeliveryService was written in a way to support transactional behaviour. Although, the dummy db does not have any transactional features due to Lists are being used in the background,
but should we add an MS SQL database behind the project we would not have to worry about transactional execution.

## Data

This project is the dummy db that only exists until the api is running. Adding SQL-like db seemed unnecessary, also I do not have it installed on my machine, this is why I chose to use
simple Lists as the data storage.
The entities describe how I would create the db schema: mainly use json fields to store data that we do not often use in search/aggregation. This is why I added AutoMapper to this project, too,
to be able to map db entities to Core entities and vica versa.

## Libraries

This project acts like a shared dll codebase that other solutions can use, too. Normally these would be packages that other solutions can reference via nuget.

The DateTimeProvider gives us the ability to easily mock it up when creating tests, so we do not have to rely on DateTime.Now or anything like that.

The db connection provider interface enforces to implement get connection feature, that is like creating a new OleDbConnection or SqlConnection. Obviously, the dummy db does not have
anything like this, this is why it simply returns a default value of IDbConnection and this is why the DbOperatingService will not use any kind of transactional execution for this dummy db.
However, for a properly implemented Sql connection all execution inside one "WithConnectionTransaction" action would be transactional. We basically pass in the whole logic as action to this
class and we only commit a transaction - if there is any - when the action execution was successful.

## Tests

This project is implemented to show how testing could be done. Both the service and validator tests use a similar IServiceProvider composition that the web api does for dependency
injection as it is much easier to setup dependencies this way.
The tests only cover a small part of the functionalities, because the subsequent tests would be very similar, so I thought they did not have any additional value. The projects show the
usage of mocks, Asserts and DI.

## Api

This project has imilar setup to Core.

The contracts are used to send/receive responses/requests, therefore they are not strongly typed (dates are string as how we represent them in a response). The contracts are mapped to/from
the Core entities via AutoMapper.

The builder gives us the list of actions based on the State of a delivery. It does not have any meaning in Core, this is why it is implemented here.

The DeliveriesController has the endpoint setup, the response setup and the error handling/logging. It does not have any king of authrozation/authentication, because that is more
difficult to setup and I dont have good enough experience in it.

The MaintenanceController has a method that can be setup on a scheduled webjob to be called and to automatically expire deliveries.

Certain settings are split between the common appsettings and the environment-specific appsettings. Date formatter is the same across all environments, but action templates are different.

# --------------------------------------------------- Original ReadMe ---------------------------------------------------------------------------------------

# GlueHome - Platform Technical Task

---

### Do not fork this repo! By doing so other candidates will be able to see your solution

---

## Background

Your company has decided to create a delivery system to connect users from the consumer market to partners from the logistics business sector.

You are responsible for building a Web API that will be used by partners and users to create, manage and execute deliveries.

### Business Requirements

* The API should support all CRUD operations (create, read, update, delete).
* A delivery must handle 5 different states: `created`, `approved`, `completed`, `cancelled`, `expired`
* Users may `approve` a delivery before it starts.
* Partner may `complete` a delivery, that is already in `approved` state.
* If a delivery is not `completed` during the access window period, then it should expire. 
* Either the partner or the user should be able to cancel a pending delivery (in state `created` or `approved`).

A delivery should respect the following structrure:

```json
{
    "state": "created",
    "accessWindow": {
        "startTime": "2019-12-13T09:00:00Z",
        "endTime": "2019-12-13T11:00:00Z"
    },
    "recipient": {
        "name": "John Doe",
        "address": "Merchant Road, London",
        "email": "john.doe@mail.com",
        "phoneNumber": "+44123123123"
    },
    "order": {
        "orderNumber": "12209667",
        "sender": "Ikea"
    }
}
```

### Technical Requirements

* All code should be written in C# and target the .NET core, or .NET framework library version 4.5+.
* Please check all code into a public accessible repository on GitHub and send us a link to your repository.

Feel free to make any assumptions whenever you are not certain about the requirements, but make sure your assumptions are made clear either through the design or additional documentation.

### Bonus
* Application logging
* Documentation
* Containerization
* Authentication
* Testing
* Data storage
* Partner facing Pub/Sub API for receiving state updates.
* Anything else you feel may benefit your solution from a technical perspective.
