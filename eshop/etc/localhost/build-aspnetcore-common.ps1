# COMMON PATHS 
$rootFolder = (Get-Item -Path "./" -Verbose).FullName

# List of solutions used only in development mode
[PsObject[]]$projectArray = @()

# type
#   build       需要编译的项目
#   service     服务项目（host、auth-service、identityserver）
#   gateway     网关项目
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../../framework/leopard/"; Name = "framework"; Type = "build"  }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/common/"; Name = "common"; Type = "build"  }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/module/account/"; Name = "account"; Type = "build"  }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/module/identity/"; Name = "identity"; Type = "build"  }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/module/saas/"; Name = "saas"; Type = "build"  }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/services/auth-server/host/EShop.AuthServer.HttpApi.Host/"; Name = "auth-server"; Type = "service"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/services/auth-server/host/EShop.AuthServer.IdentityServer/"; Name = "identityserver"; Type = "service"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/services/administration/host/EShop.Administration.HttpApi.Host/"; Name = "administration"; Type = "service"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/gateways/BackendAdminAppGateway.Host/src/"; Name = "BackendAdminAppGateway"; Type = "gateway"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/gateways/InternalGateway.Host/src/"; Name = "InternalGateway"; Type = "gateway"   }
$projectArray += [PsObject]@{ Path = $rootFolder + "/../../aspnetcore/gateways/PublicWebSiteGateway.Host/src/"; Name = "PublicWebSiteGateway"; Type = "gateway"   }


Write-host ""
Write-host ":::::::::::::: !!! You are in development mode !!! ::::::::::::::" -ForegroundColor red -BackgroundColor  yellow
Write-host "" 