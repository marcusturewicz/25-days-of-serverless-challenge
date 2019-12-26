# create a resource group
$resourceGroup = "backuprestore"
$location = "australiasoutheast"
az group create -n $resourceGroup -l $location

# create a keyvault
az keyvault create --name "$resourceGroup-keyvault" -g $resourceGroup

# create a storage account
$storageAccountName = $resourceGroup + "store"

az storage account create `
  -n $storageAccountName `
  -l $location `
  -g $resourceGroup `
  --sku Standard_LRS

# create a function app
$functionAppName = "$resourceGroup-funcapp"

az functionapp create `
  -n $functionAppName `
  --storage-account $storageAccountName `
  --consumption-plan-location $location `
  --runtime dotnet `
  -g $resourceGroup

  # assign managed identity
az functionapp identity assign -n $functionAppName -g $resourceGroup