## BYODns (bring your own dns)

__The network consists of:__
1. DNS server (example.com) - 192.168.1.100
2. Client A - 192.168.1.10
3. Client B - 192.168.1.20

The clients are domain joined.

-> VARIANT a):
Client A & Client B execute "nslookup -q=txt example.com" which returns the TXT entry/entries from the domain controller.

![variant_a](https://github.com/user-attachments/assets/eec8bfbc-b0e6-42ce-9c06-8b1f0dfccf91)


-> VARIANT b):
Client A has a local DNS server running (see [DNS-Server_1.cs)](https://github.com/Dood3/NSHookup/blob/main/nshookup_SRC/DNS-Server_1.cs). Due to using a high port "5353", no administrative rights are needed (still asks for firewall permissions).
Client B makes an according request including the IP pointing to the locally hosted DNS server: *"nslookup -q=txt example.com 192.168.1.10"*

![variant_b](https://github.com/user-attachments/assets/9d1a6142-9a11-4359-aa52-d4a4b5a97130)
