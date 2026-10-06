Currently there are only 2 data specs created and coded for in the dev environment.  These are KS2 pupil and 16-18 school.

The main notebook to run data for either data set is in databricks called - synthetic_data_KTS

There are 5 widgets at the top where you select which data you want to run.

<div>
<table>
  <thead>
    <tr>
      <td> 

![image.png](/.attachments/image-1fb6dc35-9531-41e8-9706-f0462829a4f5.png)
      </td>
    </tr>
  </thead>
</table>
</div>

| **Widget** | **Description** |
|--|--|
| data_spec | choose the source of data you want generated i.e. KS2, 16-18 |
| data_type | choose the type of data you want generated i.e. pupil or school |
| rows | select the number of rows you want to generate |
| acadm_yr | select the year for the data you want to generate|
| output_field | here you can choose to either output the headers as either the label or fieldname form the data spec |

The notebook is: [**synthetic_data_KTS - Databricks**](https://adb-2220072380334347.7.azuredatabricks.net/editor/notebooks/1661994864246563?o=2220072380334347)

The pii fields use faker to generate the fake data.  The pii fields can be identified in the data spec files in the PII column.

In the 16-18 data spec there is a history column.  This is used during processing to create columns of data with a historical year i.e if the fieldname is called TALLPUP_1618 and history = Y  and academ_yr =  2024 then there will be 4 columns generated one will have the original name then subsequent 3 years. In this example we would have 4 columns created called:-

- TALLPUP_1618
- TALLPUP_1618_23
- TALLPUP_1618_22
- TALLPUP_1618-21

If the history flag was S in the example above then we only require the historic columns and not the original column.

The establishments that are required within dev and test environments are based on live establishments but the IDs are substituted for fake establishments.  These can be seen in the establishments section within the notebook.

School files have historically been output as one single csv and pupil files are output as individual files at LA level then these are zipped up.

The generated files are output here:- [s192d01stradfdev - Microsoft Azure](https://portal.azure.com/#@platform.education.gov.uk/resource/subscriptions/212193cd-152c-4621-97d1-85cb63f025b4/resourceGroups/s192d01-dev/providers/Microsoft.Storage/storageAccounts/s192d01stradfdev/storagebrowser)

![image.png](/.attachments/image-4f094a76-56b7-4f46-8127-b1975f3d67cd.png)