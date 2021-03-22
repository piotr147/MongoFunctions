# Mongo Functions

A project for storing information about users' Lego sets.

## Technologies
* .Net Core
* Azure Functions
* Mongo DB

## How it works

User can add sets that he or she owns using Azure functions Http trigger. Details like buy timestamp or price of new set may be included. System stores information about sets like number of elements or if set is available or retired.

All data is stored in Mongo DB and it can be accessed by Azure functions Http triggers.
