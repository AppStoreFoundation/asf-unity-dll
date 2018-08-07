#bash
source setup_env_vars.sh

#ARGS_5_6=" -batchmode -projectPath=\"/Users/nunomonteiro/Documents/GitHub/AppcoinsUnityPlugin5_6/Appcoins Unity\" -logFile -quit -executeMethod ExportPackageAutomatically.ExportFromUnity"

ARGS_5_6=" -batchmode -projectPath="
ARGS_5_6+=$PROJ_5_6_PATH
ARGS_5_6+=" -logFile -quit -executeMethod ExportPackageAutomatically.ExportFromUnity"

ARGS_2017=" -batchmode -projectPath="
ARGS_2017+=$PROJ_2017_PATH
ARGS_2017+=" -logFile -quit -executeMethod ExportPackageAutomatically.ExportFromUnity"
 
ARGS_2018=" -batchmode -projectPath=\""
ARGS_2018+=$PROJ_2018_PATH
ARGS_2018+="\" -logFile -quit -executeMethod ExportPackageAutomatically.ExportFromUnity"

#export package for Unity5.6
#echo "Exporting plugin package for Unity 5.6"
#echo "Call $UNITY_5_6_PATH $ARGS_5_6"
#$UNITY_5_6_PATH $ARGS_5_6

#export package for Unity 2017
#echo "Exporting plugin package for Unity 2017"
#echo "Call $UNITY_2017_PATH $ARGS_2017"
#$UNITY_2017_PATH $ARGS_2017

#export package for Unity 2018
echo "Exporting plugin package for Unity 2018"
#echo "Call $UNITY_2018_PATH $ARGS_2018"
$UNITY_2018_PATH $ARGS_2018