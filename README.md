# TraineeshipForum
To Run in Production Mode:

In your Solution Explorer select project and right-click to select Publish <br>
Select Folder and click Next <br>
Select "Folder Location" and Click Finish <br>
Click Publish <br>
Open IIS <br>
Right click on "Sites" -> "Add Website..." <br>
Select "Site name" and pick physical path equal to "Folder Location", dont forget to use free port <br>
Go to Application Pools <br>
Right click on Application Pool name, which is the same as your "<Site Name>" and select Advanced Settings <br>
Make Identity equal "Local System" in Process Model <br>
Right Click on "<Site Name>", select "Manage Application" and click "Browse" <br>






