# DogsIRL
![Main Logo](https://github.com/DogsIRLOrg/DogsIRL/blob/dev/assets/main.png)

## 🐶 Deployment
Our API is live on Azure!

https://dogs-irl-api.azurewebsites.net/api/account/register

## 🐶 Authors
- Andrew Casper
- Carrington Beard
- Teddy Damian
- Brody Rebne

## 🐶 Problem Domain
DogsIRL is a social platform for pets to interact with other pets. People like pet pictures and want to interact with them more. Pets increase happiness and emotional wellbeing. Login page, upload info/details: name, playfulness, fluffiness, snugability, vegetarian/carnivore, etc. Park page for interaction to view other puppies.
Stretch goals: Add other animals, Rating system Interact with games

## 🐶 Software Requirements
### 🐾 **Vision**
Here at Dog IRL we believe pets should have an online presence and ability to meet and make new friends. People love showing off their adorable pets and with Dog IRL now they have a social media outlet that allows them to do so. With this app your pets can meet new friends and collect their ‘pet cards’ to show how all the pets are connected.

### 🐾 **Scope In**
This web app will allow users to create profiles for their dogs.
The app will allow pets to virtually interact with each other.
User will be able to collect the petcards of fellow pets once they have interacted.
Pets will be able to have short text dialogue.

### 🐾 **Scope Out**
Our mobile app will never sell the data of our users.

### 🐾 **Functional Requirements**
1. A user can create a profile and create a pet for that profile.
2. A user can have their pet interact with another users' pet in the virtual 'dog park'
3. A user can collect petcards from the other dog for their own collection.

### 🐾 **Data Flow**
	
A user will sign up and create a profile. At this point a user can create a 'pet card' and upload an image and assign stats for their pet. This pet card can be taken to the virtual dog park and have an interaction with another user's dog. The user then can collect the other dog's petcard to constantly remind them of that memorable interaction. Users can log-out or create additional pet cards as they choose.

### 🐾 **Non-Functional Requirements**

Security - Protecting users emails and passwords as they are stored. We will use ASP.NET Core Identity to store users information and hash passwords. Users will be able to sign up and create a profile and not worry about their information being stolen.

## 🐶 ERD Diagram
![ERD Diagram](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/DogsIRL%20ER%20Diagram.png)
*  *  *  *  *
## 🐶 Wireframe
![Wireframe](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/DogsIRL%20Wireframes.png)
*  *  *  *  *
## 🐶 Domain Modeling
![DomainModeling](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/DogsIRLDomainModel.png)
*  *  *  *  *
## 🐶 Forgot Password Structure
![ForgotPassword](https://github.com/401FinalProjectOrg/DogsIRL/blob/dev/Forgot-password.png)
*  *  *  *  *

#### 🔨 Patch Notes
May 22 - 2020 || App Created.

July 8 - 2020 || App Re-deployed to Azure with security improvements and increased account functionality

#### ☕ Resources
- Blob Storage - https://www.c-sharpcorner.com/article/xamarin-forms-upload-image-to-blob-storage/
- JSON Web Token - https://www.c-sharpcorner.com/article/asp-net-core-web-api-creating-and-validating-jwt-json-web-token/
- StackOverflow - https://stackoverflow.com/
- Microsoft Docs Xamarin Forms - https://docs.microsoft.com/en-us/xamarin/xamarin-forms/
