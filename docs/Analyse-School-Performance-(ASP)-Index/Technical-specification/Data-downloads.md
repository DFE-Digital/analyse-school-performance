[[_TOC_]]

### File Storage in Blob Storage Overview

Files available for download are stored in Azure Blob Storage. There are two primary sources of files:
1.  **Analyse School Performance (ASP)**  
    Historically, ASP files were generated dynamically on request. However, in ASP 2.0, files are pre-generated and stored in blob storage to reduce per-request processing. This approach leverages the existing data pipeline infrastructure to process and store ASP files efficiently.
    
2.  **Key To Success (KTS)**  
    KTS files have always been stored in blob storage and follow a similar structure to ASP files.
    

#### File Scopes and Structure

Files are available at two scopes:
*   **School Scope**: Contains data specific to a school (e.g., pupil-level or aggregated school-level data).
*   **Local Authority (LA) Scope**: Contains data for all pupils in an LA or aggregated at the LA level.
The file structure in blob storage is organized as follows:
*   **ASP Container**
    
        ├─ School
        │  ├─ {URN}
        │  └─ (more URNs)
        └─ LA
           ├─ {LA Code}
           └─ (more codes)
        
    
*   **KTS Container**
    
        ├─ School
        │  ├─ {URN}
        │  └─ (more URNs)
        └─ LA
           ├─ {LA Code}
           └─ (more codes)
        
    
Within each School or LA folder, files are further organized by year and file type (CSV, TSV, XLSX). For example:

    KTS Container
    School
    └─ 123456
       ├─ 2023
       │  ├─ csv
       │  │  ├─ ks2_pupil_provisional.csv
       │  │  ├─ ks2_pupil_revised.csv
       │  │  └─ (more files)
       │  ├─ tsv
       │  │  ├─ ks2_pupil_provisional.tsv
       │  │  ├─ ks2_pupil_revised.tsv
       │  │  └─ (more files)
       │  └─ xlsx
       │     ├─ ks2_pupil_provisional.xlsx
       │     ├─ ks2_pupil_revised.xlsx
       │     └─ (more files)
       └─ 2024
          ├─ csv
          ├─ tsv
          └─ xlsx
    

#### File Naming Conventions

Files are named based on:
*   Dataset type (e.g., `ks2_pupil`, `post16_school`)
*   Year (e.g., `2023`, `2024`)
*   Version (e.g., `provisional`, `revised`, `final`)
*   File type (e.g., `csv`, `tsv`, `xlsx`)
For example:
*   `ks2_pupil_provisional.csv`
*   `post16_school_revised.tsv`

### UI Representation of Downloads Overview

In the UI, files are grouped by dataset type (e.g., "Key Stage 2", "QLA") and displayed with user-friendly labels. Each file is associated with a logical download object that includes:
*   **Source**: ASP or KTS
*   **Label**: User-friendly name (e.g., "KS2 pupil")
*   **Dataset Type**: Logical grouping (e.g., "Key stage 2")
*   **Year**: Year of the data
*   **Version**: Provisional, Revised, or Final
*   **File Type**: CSV, TSV, or XLSX

#### Download Object Definition

A download object is defined as:

    {
      "id": "string",
      "source": "string",
      "label": "string",
      "dataSetType": "string",
      "year": "number",
      "version": "string"
    }
    

### Download Configuration Overview
 
To map file paths in blob storage to download objects, a `downloads-config.json` file is stored in the `config` container. This file contains an array of configuration objects, each representing a specific type of download.
Example configuration:

    [
      {
        "id": "kts-school-ks2-pupil",
        "source": "KTS",
        "scope": "School",
        "dataSetType": "KeyStage2",
        "label": "KS2 pupil",
        "filePathPattern": "School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}"
      },
      {
        "id": "asp-school-ks4-school",
        "source": "ASP",
        "scope": "School",
        "dataSetType": "KeyStage4",
        "label": "KS4 school",
        "filePathPattern": "School/{urn}/{year}/{filetype}/ks4_school_{version}.{filetype}"
      },
      ...
    ]
    

#### File Path Patterns

File path patterns are used to identify and map files to their corresponding download objects. Examples:
*   `School/{urn}/{year}/{filetype}/ks2_pupil_{version}.{filetype}` → KS2 pupil
*   `School/{urn}/{year}/{filetype}/post16_school_{version}.{filetype}` → 16-18 school
*   `LA/{laCode}/{year}/{filetype}/ks2_la_{version}.{filetype}` → KS2 LA

#### Download ID Construction

The download ID is constructed as:

    {download-config.id}-{year}-{version}
    

For example:
*   `kts-school-ks2-pupil-2023-provisional`
*   `asp-school-ks4-school-2024-revised`
This ID is used in the UI to uniquely identify downloads and locate the corresponding file in blob storage.

### API Endpoints Overview

The download service provides two main endpoints:
*   `/api/downloads` - Gets available downloads
*   `/api/downloads/package` - Downloads selected files as a package

#### GET /api/downloads

Retrieves available downloads for a specified scope (School or LA).

##### Query Parameters

*   `scope` (required): Either "School" or "LA"
*   `scopeId` (required): School URN or LA code
*   `year` (optional): 4-digit year to filter results

##### Response Format

    {
      "Downloads": [
        {
          "Id": "string",
          "Source": "string",
          "Label": "string",
          "DatasetType": "string",
          "Year": number,
          "Version": "string"
        }
      ]
    }
    

##### Error Responses

*   400 Bad Request
    *   Missing or invalid parameters
    *   Invalid scope
    *   Invalid year format
*   404 Not Found
    *   School/LA not found
    *   No downloads available
*   500 Server Error
    *   Invalid configuration
    *   Missing configuration file

#### GET /api/downloads/package

Downloads selected files as a ZIP package.

##### Query Parameters

*   `scope` (required): Either "School" or "LA"
*   `scopeId` (required): School URN or LA code
*   `fileType` (required): File format (CSV, TSV, or XLSX)
*   `downloadIds` (required): One or more download IDs

##### Download ID Format

`{download-config.id}-{identifier}-{year}[-{version}]`
Example: `kts-school-ks2-pupil-123456-2024-provisional`

##### Response

*   200: ZIP file containing requested downloads
*   Filename format: `YYYYMMDD_HHMMSS_download.zip`

##### Error Responses

*   400 Bad Request
    *   Missing or invalid parameters
    *   Invalid file type
    *   Invalid download ID format
*   403 Not Allowed
    *   Requested files not accessible within scope
*   404 Not Found
    *   Files not found in storage
*   500 Server Error
    *   Configuration errors

#### Supported Values

##### File Types

*   CSV (case insensitive)
*   TSV (case insensitive)
*   XLSX (case insensitive)

##### Dataset Types

*   Key stage 2 (KS2)
*   Key stage 4 (KS4)
*   16-18
*   QLA (year 6 only)
*   School characteristics
*   Absence
*   Exclusions
*   Multiplication table check (MTC)
*   Phonics

##### Versions

*   Provisional
*   Revised
*   Final
*   With variations:
    *   `[version], with CLA`
    *   `[version], without CLA`

##### Sources

*   Analyse school performance
*   Key to success
