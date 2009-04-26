@echo off

if not exist "%1Tools\Simian\bin\simian-2.2.24.exe" goto no-simian

%1Tools\Simian\bin\simian-2.2.24.exe -reportDuplicateText+ -formatter=xml:"%1Metrics\SimilarityReport.simian" -threshold=3 -excludes="%1**\*.Designer.cs" -excludes="%1**\AssemblyInfo.cs" -failOnDuplication- "%1Source\**\*.cs"

goto finish

:no-simian
echo Could not find Simian
echo Get Simian from http://www.redhillconsulting.com.au/products/simian/
echo Then extract the contents to the %1Tools\Simian folder
echo (The simian-2.2.24.exe executable should be located in the
echo  %1Tools\Simian\bin folder)
echo NOTE: The free license is only for use on open-source and 
echo  non-commercial/non-government projects
echo  (Such as MaVeriCk)

:finish