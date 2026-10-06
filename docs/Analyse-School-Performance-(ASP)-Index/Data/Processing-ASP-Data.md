The data processing as done automatically from adf pipelines. The data is read from the following locations by a trigger when a new file is copied into it.

<div>
<table>
  <tr>
    <td markdown="span" style="font-weight: bold; border: none">ASP:</td>
    <td markdown="span" style="padding: 0.1vw; border: none">/data/blobs/DataProcessing/DownloadData/upload</td>
  </tr>
  <tr>
    <td markdown="span" style="border: none"></td>
    <td markdown="span" style="padding: 0.1vw; border: none">- Trigger name: download_data_asp</td>
  </tr>
  <tr>
    <td markdown="span" style="border: none"></td>
    <td markdown="span" style="padding: 0.1vw; border: none">- Triggered pipeline: Download_data_asp</td>
  </tr>
</table>
</div>

The pipeline gets all the files in the zip, extracts them processes the pupil file for download purposes and discards the any other files within the zip.  After each data stage there is success or failure notification.  The notification is sent by a logic app.

During the pupil file processing a temporary file is created that tracks the name and location of each created file and this is used in later stages to copy from the processing storage to website storage.

![image.png](/.attachments/image-f356f150-298a-4392-b2e4-0fd46376be3e.png)

The main notebook for processing the pupil file is: [DataDownloads](https://adb-2220072380334347.7.azuredatabricks.net/editor/notebooks/2623413016442675?o=2220072380334347#command/2623413016442676) The pupil file is split by 2 groups LA's and schools(URN). There are 3 versions of each file created, a csv file , a tsv file and an excel file. The files are out put to here: [s192d01stradfdev - output](https://portal.azure.com/#@platform.education.gov.uk/resource/subscriptions/212193cd-152c-4621-97d1-85cb63f025b4/resourceGroups/s192d01-dev/providers/Microsoft.Storage/storageAccounts/s192d01stradfdev/storagebrowser)

![image.png](/.attachments/image-d06c6329-2df7-42a3-b67a-bc5ce30835bb.png)

Then the latter pipeline stages copy them to here: [s192d01strdev - downloads-asp](https://portal.azure.com/#@platform.education.gov.uk/resource/subscriptions/212193cd-152c-4621-97d1-85cb63f025b4/resourceGroups/s192d01-dev/providers/Microsoft.Storage/storageAccounts/s192d01strdev/storagebrowser)

![image.png](/.attachments/image-ce9277ba-1621-47d5-90b6-20d88c13325c.png)