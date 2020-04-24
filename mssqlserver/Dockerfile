FROM microsoft/mssql-server-linux:latest
# Create work directory
RUN mkdir -p /usr/work
WORKDIR /usr/work
# Copy all scripts into working directory
COPY . /usr/work/
# Grant permissions for the import-data script to be executable
#RUN chmod +x /usr/work/import-data.sh
RUN apt-get update && \
    apt-get install dos2unix && \
    apt-get clean

RUN dos2unix /usr/work/import-data.sh && apt-get --purge remove -y dos2unix && rm -rf /var/lib/apt/lists/*

EXPOSE 1433
CMD /bin/bash ./entrypoint.sh