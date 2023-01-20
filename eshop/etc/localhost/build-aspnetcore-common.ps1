# COMMON PATHS 
$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions used only in development mode
[PsObject[]]$projectArray = @()

# type
#   build       需要编译的项目
#   service     服务项目（host、identity、identityserver）
#   gateway     网关项目

#leopard
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../../framework/leopard/"; Name = "framework"; Type = "build"  }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../../framework/leopard/modules/identity/"; Name = "leopard-identity"; Type = "build"  }

#eshop
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/common/"; Name = "common"; Type = "build"  }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/modules/account/"; Name = "account"; Type = "build"  }
#$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/modules/saas/"; Name = "saas"; Type = "build"  }

# service
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/services/identity/host/EShop.Identity.HttpApi.Host/"; Name = "identity"; Type = "service"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/services/identity/host/EShop.Identity.AuthServer/"; Name = "identity-auth-server"; Type = "service"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/services/administration/host/EShop.Administration.HttpApi.Host/"; Name = "administration"; Type = "service"   }

#gateway
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/gateways/BackendAdminAppGateway.Host/src/"; Name = "BackendAdminAppGateway"; Type = "gateway"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/gateways/InternalGateway.Host/src/"; Name = "InternalGateway"; Type = "gateway"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/gateways/PublicWebSiteGateway.Host/src/"; Name = "PublicWebSiteGateway"; Type = "gateway"   }


Write-host ""
Write-host ":::::::::::::: !!! You are in development mode !!! ::::::::::::::" -ForegroundColor red -BackgroundColor  yellow
Write-host "" 