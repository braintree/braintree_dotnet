.PHONY: console mono build build-mono build-core3

console: core3

mono: build-mono
	docker run -it -v="$(PWD):/home/admin/bt/braintree-dotnet" --net="host" braintree-dotnet-mono /bin/bash -l

core3: build-core3
	docker run -it -v="$(PWD):/home/admin/bt/braintree-dotnet" --net="host" braintree-dotnet-core3 /bin/bash -l -c "dotnet restore;bash"

build: build-mono build-core3

build-mono:
	docker build -t braintree-dotnet-mono -f Dockerfile-mono .

build-core3:
	docker build -t braintree-dotnet-core3 -f Dockerfile-core3 .
