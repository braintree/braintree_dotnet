.PHONY: console mono core core2 build build-mono build-core build-core2

console: core2

mono: build-mono
	docker run -it -v="$(PWD):/home/admin/bt/braintree-dotnet" --net="host" braintree-dotnet-mono /bin/bash -l

core: build-core
	docker run -it -v="$(PWD):/home/admin/bt/braintree-dotnet" --net="host" braintree-dotnet-core /bin/bash -l -c "dotnet restore;bash"

core2: build-core2
	docker run -it -v="$(PWD):/home/admin/bt/braintree-dotnet" --net="host" braintree-dotnet-core2 /bin/bash -l -c "dotnet restore;bash"

build: build-mono build-core

build-mono:
	docker build -t braintree-dotnet-mono -f Dockerfile-mono .

build-core:
	docker build -t braintree-dotnet-core -f Dockerfile-core .

build-core2:
	docker build -t braintree-dotnet-core2 -f Dockerfile-core2 .
