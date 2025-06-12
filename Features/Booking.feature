Feature: Booking API

  Background:
    Given base URL "https://restful-booker.herokuapp.com"

  Scenario Outline: Create and retrieve booking
    When I create a booking with:
      | FirstName | LastName | TotalPrice | DepositPaid  | Checkin      | Checkout     | AdditionalNeeds |
      | <FirstName> | <LastName> | <TotalPrice> | <DepositPaid> | <Checkin> | <Checkout> | <AdditionalNeeds> |
    Then the API response status is 200
    And the booking can be retrieved with price <TotalPrice>

    Examples:
      | FirstName | LastName | TotalPrice | DepositPaid | Checkin     | Checkout     | AdditionalNeeds | BookingId  |
      | John      | Doe      | 150        | true        | 2025-06-12  | 2025-06-15   | Breakfast       |            |
      | Jane      | Smith    | 200        | false       | 2025-07-01  | 2025-07-03   | Dinner          |            |