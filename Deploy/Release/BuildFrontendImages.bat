docker rmi mobilefrontend:latest adminfrontend:latest
docker system prune -f
cd ../../
cd Public/WebPage/Admin
docker build . -t adminfrontend:latest 
cd ../www 
docker build . -t mobilefrontend:latest