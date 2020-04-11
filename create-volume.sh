docker volume create --driver local --opt type=nfs --opt device=./volumes/esdata1  esdata1
docker volume create --driver local --opt type=nfs --opt device=./volumes/esdata2  esdata2
docker volume create --driver local --opt type=nfs --opt device=./volumes/esdata3  esdata3
docker volume create --driver local --opt type=nfs --opt device=./volumes/mongodata  mongodata
docker volume create --driver local --opt type=nfs --opt device=./volumes/httpCall  httpCall

