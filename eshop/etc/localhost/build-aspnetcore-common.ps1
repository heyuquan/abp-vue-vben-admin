# COMMON PATHS 
$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions used only in development mode
[PsObject[]]$projectArray = @()

# type
#   build       需要编译的项目
#   service     服务项目（host、identity、identityserver）
#   gateway     网关项目

#leopard
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../../framework/leopard"; RunPath = ""; Name = "framework"; Type = "build"  }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../../framework/modules/identity"; RunPath = ""; Name = "leopard-identity"; Type = "build"  }

#eshop
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/common/"; RunPath = ""; Name = "common"; Type = "build"  }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/modules/account"; RunPath = ""; Name = "account"; Type = "build"  }
#$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/modules/saas/"; Name = "saas"; Type = "build"  }

#service
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/services/identity"; RunPath = "/host/EShop.Identity.HttpApi.Host"; Name = "identity"; Type = "service"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/services/identity"; RunPath = "/host/EShop.Identity.AuthServer"; 
                              Name = "identity-auth-server"; Type = "service"; 
                              IsMigration = $true ; 
                              EfProject = "./src/EShop.Identity.EntityFrameworkCore/EShop.Identity.EntityFrameworkCore.csproj" ;
                              StartProject = "./host/EShop.Identity.HttpApi.Host/EShop.Identity.HttpApi.Host.csproj"
                            }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/services/administration"; RunPath = "/host/EShop.Administration.HttpApi.Host"; 
                              Name = "administration"; Type = "service"   
                              IsMigration = $true ; 
                              EfProject = "./src/EShop.Administration.EntityFrameworkCore/EShop.Administration.EntityFrameworkCore.csproj" ;
                              StartProject = "./host/EShop.Administration.HttpApi.Host/EShop.Administration.HttpApi.Host.csproj"
                            }

#gateway
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/gateways/AdministrationGateway.Host"; RunPath = "/host"; Name = "AdministrationGateway"; Type = "gateway"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/gateways/InternalGateway.Host"; RunPath = "/host"; Name = "InternalGateway"; Type = "gateway"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/gateways/PublicWebGateway.Host"; RunPath = "/host"; Name = "PublicWebGateway"; Type = "gateway"   }

foreach ($project in $projectArray) {  
    $project.RunPath = $project.Path + $project.RunPath

    #、相对路径，转绝对路径  ps：xcopy的时候识别不了相对路径
    $project.Path = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($project.Path)
    $project.RunPath = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($project.RunPath)
}

#test
# foreach ($project in $projectArray) {  
#     Write-Host $project.Path
#     Write-Host $project.RunPath
#     Write-Host $project.Name
#     Write-Host $project.Type
# }

Write-host ""
Write-host ":::::::::::::: !!! You are in development mode !!! ::::::::::::::" -ForegroundColor red -BackgroundColor  yellow
Write-host "" 