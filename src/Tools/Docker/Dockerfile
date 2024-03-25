#------------------------------------------------+
# Create image with SDK to build the application |
#------------------------------------------------+
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /source

# Add Node.js build kit
ENV NODE_VERSION 18.2.0
ENV NODE_DOWNLOAD_SHA 73d3f98e96e098587c2154dcaa82a6469a510e89a4881663dc4c86985acf245e
ENV NODE_DOWNLOAD_URL https://nodejs.org/dist/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.gz
RUN wget "$NODE_DOWNLOAD_URL" -O nodejs.tar.gz \
	&& echo "$NODE_DOWNLOAD_SHA  nodejs.tar.gz" | sha256sum -c - \
	&& tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
	&& rm nodejs.tar.gz \
	&& ln -s /usr/local/bin/node /usr/local/bin/nodejs \
	&& curl -sL https://deb.nodesource.com/setup_16.x |  bash - \
	&& apt update \
	&& apt-get install -y nodejs

# Copy source code
COPY / .

# Build application
ENV SpaSkipBuild true
RUN dotnet publish UI/Akkordplus/deftq.Akkordplus.Webapplication/deftq.Akkordplus.WebApplication.csproj --configuration Release --output output


RUN mkdir -p /source/dev-ssl-cert
RUN dotnet dev-certs https -ep /source/dev-ssl-cert/deftq_api_dev_cert.pfx -p ssl-password

#-----------------------------------------------+
# Create image based on runtime without the SDK |
#-----------------------------------------------+
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build-env /source/output .
COPY --from=build-env /source/dev-ssl-cert/deftq_api_dev_cert.pfx /https/

ENTRYPOINT ["./deftq.Akkordplus.WebApplication"]

