# Azure Cognitive Services Integration API
## Problem Overview

This project solves the challenge of automatically extracting personal details from Uganda National IDs and performing facial matching as part of a Know Your Customer (KYC) SaaS solution. Our initial attempts using Tesseract OCR failed due to poor customization support and time limitations.

## Complexity and Solution
Extracting accurate text from Uganda's National IDs and implementing reliable facial recognition posed significant
challenges, especially given image quality inconsistencies and limited documentation. After researching available tools, we adopted Azure Cognitive Services for OCR and face matching.

Due to regional restrictions on the Face API,
mock responses were used during testing and for demonstration purposes (e.g., during interviews). The OCR API works without restrictions and has been successfully integrated for ID data extraction.

## How to Run the Application
### Prerequisites
- .NET 9 SDK

- An Azure Cognitive Services account

- IDE: Visual Studio 2022+ or JetBrains Rider 2023+

- Optional: Postman or Insomnia client for testing the API

## Setup Instructions
1. Clone the Repository
```
git clone https://github.com/Mwesigwa-Enock/azure-ai-api-connector.git
```
2. Configure Azure Credentials
```
"Cognitive": {
    "Endpoint": "azure-cognitive-link",
    "ApiKey": "api-key"
  },
  "Vision": {
    "Endpoint": "vision-api-link",
    "ApiKey": "api-key",
    "Mock": true
  }
```
### Running the Project
#### Using .NET CLI
```
dotnet restore
dotnet run
```
#### Using JetBrains Rider
- Open the project folder.
- Let Rider detect and load the .csproj file.
- Click Run or use Shift+F10.

#### Using Visual Studio
- Open the solution file.
- Set the startup project.
- Press F5 to run with debugging or Ctrl+F5 to run without.

## API Usage
Once running, test the Cognitive endpoints by sending a POST request to:

- OCR (Document Analysis API)
  ```
  POST http://localhost:5218/api/v1/documents/analyze
  ```
  - Request Body
    - multipart/form-data with:
      - DocumentFile (ID image)
      - Type (Face for ID - NationalIdBack or NationalIdFront)

- Face Matching API
  ```
  POST http://localhost:5218/api/v1/vision/facial-comparison
  ```
    - Request Body
      - multipart/form-data with:
        - image1
        - image2



## Notes
- **Mock Face API:** Due to Azure regional restrictions, the Face API is mocked in the current build to simulate the face matching process for interviews and demonstrations.
- **OCR API:** Azure OCR is fully functional and integrated for real data extraction.

## References
- [Azure Cognitive Services OCR](https://docs.microsoft.com/azure/cognitive-services/computer-vision/concept-recognizing-text)

- [Azure Computer Vision API](https://docs.microsoft.com/azure/cognitive-services/computer-vision/overview)

- [Vision API Limitations](https://docs.microsoft.com/azure/cognitive-services/computer-vision/faq#limitations)