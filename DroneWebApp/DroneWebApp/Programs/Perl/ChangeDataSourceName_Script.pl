#!/usr/bin/env perl -i

use Sys::Hostname;

# Fetch the computer name
$name = hostname
print(hostname, "\n");

$file ="../../Web.config";

# Read file
open(IN, '<', $file) or die $!;
$/ = undef;
my $all = <IN>;
close IN;

# Replace
$all =~ s/([dD]ata [sS]ource=)(.*?)(;)/$1$name$3/g;

# Write file
open(OUT, '>', $file) or die $!;
print OUT $all;
