# ListFiles v. 0.1
  Utility that searches for files by name, listing all found with FullName that contains the PublicKeyToken and version.
  Also possible to filter on publicKeyToken and/or version.
  
  Usage:
     ListFiles [rootDirectory] [fileName] [-p:publicKeyToken] [-v:version]
  
  Example:
     ListFiles C:\projects log4net.dll -p:692fbea5521e1304 -v:1.2.10.0
     
