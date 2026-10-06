The data processing as done automatically from adf pipelines. The data is read from the following locations by a trigger when a new file is copied into it.

<div>
<table>
  <tr>
    <td markdown="span" style="font-weight: bold; border: none">KTS:</td>
    <td markdown="span" style="padding: 0.1vw; border: none">/data/blobs/DataProcessing/asp-kts-processing/pre-processing/upload/</td>
  </tr>
 <tr>
   <td markdown="span" style="border: none"></td>
   <td markdown="span" style="padding: 0.1vw; border: none">- Trigger name: trg_KTS</td>
</tr>
  <tr>
    <td markdown="span" style="border: none"></td>
    <td markdown="span" style="padding: 0.1vw; border: none">- Triggered pipeline: KTS_Master</td>
  </tr>
</table>
</div>


![image.png](/.attachments/image-2d01a3dc-4880-4ae1-8e66-626f1a3366a1.png)

This pipeline is the master for KTS and calls series of other pipelines passing the appropriate parameters.

At the moment the data is just output to 