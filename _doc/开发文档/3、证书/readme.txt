https://blog.csdn.net/lixiaowei16/article/details/72639817

genrsa -out mkdemo.key 1024
req -new -x509 -key mkdemo.key -out mkdemo.cer -days 3650 -subj /CN=134.175.121.78

pkcs12 -export -in mkdemo.cer -inkey mkdemo.key -out mkdemo.pfx


密码：123456