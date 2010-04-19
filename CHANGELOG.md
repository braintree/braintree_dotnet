## 1.2.1

* Escape all XML
* Updated quick start example in README

## 1.2.0

* Added subscription search
* Return associated subscriptions when finding credit cards
* Added option to change default credit card for a customer
* Added an All method to ValidationErrors to return List of all errors at that level
* Added a DeepAll method to ValidationErrors to return List of all errors at that level and all errors below
* Renamed DeepSize() to DeepCount
* Added ProcessorAuthorizationCode to Transaction
* Allow setting MerchantAccountId when creating or updating subscriptions
* Updated ForObject to return an empty ValidationErrors object instead of null if there are no errors
* Raise down for maintenance exception instead of forged query string when down for maintenance
* Fixed bug in TotalPages if there are zero total items

