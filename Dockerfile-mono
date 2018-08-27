FROM debian:jessie

RUN apt-get update
RUN apt-get -y install rake gnupg curl

RUN apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
RUN echo "deb http://download.mono-project.com/repo/debian jessie main" | tee /etc/apt/sources.list.d/mono-official.list
RUN apt-get update

RUN apt-get -y install mono-devel mono-complete mono-dbg referenceassemblies-pcl ca-certificates-mono mono-xsp4 mono-xbuild mono-vbnc binutils fsharp nuget libnewtonsoft-json-cil-dev libnewtonsoft-json5.0-cil

RUN apt-get -y install curl libunwind8 gettext apt-transport-https
RUN curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
RUN mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg

RUN echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-debian-jessie-prod jessie main" | tee /etc/apt/sources.list.d/dotnetdev.list
RUN apt-get update
RUN apt-get -y install dotnet-sdk-2.0.0

WORKDIR /braintree-dotnet
