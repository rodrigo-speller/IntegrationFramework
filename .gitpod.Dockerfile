FROM mcr.microsoft.com/dotnet/core/sdk:2.2

RUN apt-get update -y \
    && echo "deb http://www.rabbitmq.com/debian/ testing main" | tee /etc/apt/sources.list.d/rabbitmq.list \
    && wget -O- https://www.rabbitmq.com/rabbitmq-release-signing-key.asc | apt-key add - \
    && apt-get install apt-transport-https -y \
    && apt-get update -y \
    && apt-get install rabbitmq-server -y

CMD .gitpod.init