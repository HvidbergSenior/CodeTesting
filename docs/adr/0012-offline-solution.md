# 1. Record architecture decisions

Date: 2023-16-03

## Status

Decided

## Context

In order to accomodate situations, where the user may find themselves in locations without internet access, we have proposed an offline solution.

The offline solution needs to have a realistic time-estimate, which requires the solution to stay simple and avoid time-consuming use-cases like online-offline synchronization conflicts.

## Decision

We decided upon a solution built upon the following restrictions and assumptions:

Restrictions:

- During offline use, the only functionality accessible to the user is adding measurements

- Adding measurements offline is restricted to selecting the operation/material from the list of favorites

- The user can only start the application while being connected to the internet. The user must then login and select a project before offline-mode can be activated.

- The user can not change project while in offline mode.

- The measurements added, while in offline mode, will be saved locally on the device. Hence, these measurements are only accessible on the device until moved to the project, which can be done when internet connection is reestablished.

- The list of favorite materials/operations is saved on the device locally and must be synchronized with the online favorites list once in a while.

Assumptions:

- It is acceptable that a user's flow is interrupted when the internet connection is lost. Since the work flows in Akkord+ are relatively short, we estimate that the loss of work in such an interruption will be minimal.

## Consequences

The offline solution based on the above restrictions and assumptions consists of 4 parts:

- Automatic detection of online/offline mode with a circuit breaker pattern to ensure certainty of offline status. A flag is accesible through a useOnlineStatus hook.

- Dialog box that appears when the user is offline. The dialog box informs the user of the loss of internet and offers two solutions: Find internet connection again or continue in Draft.

- The Draft page allows the user to add measurements while in offline mode. Measurements added in the draft are saved locally and are specific to the current project. They will have to be moved to the project folders when online in order to count towards the salary.

- An expanded favorites list is introduced. This list is saved locally on the device and consists of all materials and operations on the favorites list. Relevant assembly-codes, supplements and so on are also fetched and saved on the device to allow for adding measurements with these extra informations. We opted for a solution where we every x minutes synchronize the favorites list up against the database in order to ensure that the newest favorites are added to the local list.
