. "./build-aspnetcore-common.ps1"

# Build all solutions
foreach ($project in $projectArray) {  
    # # 测试
    # if($project.Name -ne "administration") {
    #     continue;
    # }

    Write-Host "[ $($project.Name) ] begin copy common-file , project-path:$($project.Path)" -ForegroundColor yellow

    if (Test-Path $project.Path) {   
        $cur_sln_folder = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$($rootFolder)/aspnetcore/common-file/sln/*.*")
        Write-Host xcopy $cur_sln_folder "$($project.Path)\" /s/i/y/d
        &xcopy $cur_sln_folder "$($project.Path)\" /s/i/y/d
        Write-Host "success copy common-file/sln..." -ForegroundColor green

        if($project.Type -eq "service") {
            $cur_run_folder = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("$($rootFolder)/aspnetcore/common-file/run/*.*")
            Write-Host xcopy $cur_run_folder "$($project.RunPath)\" /s/i/y/d
            &xcopy $cur_run_folder "$($project.RunPath)\" /s/i/y/d
            Write-Host "success copy common-file/run..." -ForegroundColor green
        }
    } else {
        Write-Host "can not find path:$($project.Path)" -ForegroundColor red
    }
    Write-Host "" # 换行
}

Set-Location $rootFolder

pause