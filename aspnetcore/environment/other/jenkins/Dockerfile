# 安装jenkins时，同时把dotnet、node、powershell 工具一起安装
# jenkins:lts 镜像会自带git，所以不用再单独安装git

FROM jenkins/jenkins:lts
#设置当前用户为root
USER root

WORKDIR /dotnet
#更改debian源为阿里云源
RUN sed -i s/deb.debian.org/mirrors.aliyun.com/g /etc/apt/sources.list
RUN apt-get update 
RUN apt-get install wget
RUN apt-get install --assume-yes apt-utils

#安装dotnet6
#前往微软官网查看需要的版本https://dotnet.microsoft.com/download
RUN wget -O dotnet.tar.gz https://download.visualstudio.microsoft.com/download/pr/1d2007d3-da35-48ad-80cc-a39cbc726908/1f3555baa8b14c3327bb4eaa570d7d07/dotnet-sdk-6.0.403-linux-x64.tar.gz
#解压sdk到当前目录/dotnet，这是因为上面设置了工作目录
RUN tar zxf dotnet.tar.gz -C ./
RUN rm -rf dotnet.tar.gz
#把dotnet目录和dotnet tools目录添加到环境变量PATH，这样就可以使用dotnet命令了
#PATH="$PATH:your path1:your path2 …"
ENV PATH="${PATH}:/dotnet:/var/jenkins_home/.dotnet/tools"
#设置DOTNET_ROOT变量
ENV DOTNET_ROOT="/dotnet"


RUN apt update -y
#安装liunx常用工具 (icu-devtools是运行dotnet需要)
RUN apt install icu-devtools vim zip unzip -y

WORKDIR /node
# 使用脚本 Nodesource快速安装nodejs环境
# https://github.com/nodesource/distributions
RUN curl -fsSL https://deb.nodesource.com/setup_19.x | bash -
# get这个包，可能耗时比较久，没有进度反馈，多等一会就好
# Get:1 https://deb.nodesource.com/node_19.x bullseye/main amd64 nodejs amd64 19.2.0-deb-1nodesource1 [29.4 MB]
RUN apt-get install -y nodejs 

WORKDIR /powershell
# # 安装powershell
# # https://learn.microsoft.com/zh-cn/powershell/scripting/install/install-ubuntu?view=powershell-7.3
# Update the list of packages
RUN apt-get update
# Install pre-requisite packages.
RUN apt-get install -y wget apt-transport-https software-properties-common
# Download the Microsoft repository GPG keys
RUN wget -q "https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb"
# Register the Microsoft repository GPG keys
RUN dpkg -i packages-microsoft-prod.deb
# Update the list of packages after we added packages.microsoft.com
RUN apt-get update
# Install PowerShell
# get这个包[71.7 MB]，可能耗时比较久，没有进度反馈，多等一会就好
# Get:1 https://packages.microsoft.com/ubuntu/22.04/prod jammy/main amd64 powershell amd64 7.3.0-1.deb [71.7 MB]
RUN apt-get install -y powershell
# Start PowerShell
RUN pwsh

#修改jenkins用户到root附加组
RUN usermod -a -G root jenkins
#设置当前用户为jenkins
USER jenkins

WORKDIR /var/jenkins_home