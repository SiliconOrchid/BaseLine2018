This "Entity" folder is used to store database entities.

These are plain POCO objects, they will not have any data-annotations.   

Database-specific meta-data is defined using fluent-api (in the data project).

As a convention, it is recommended that you suffix classnames as a localised
convention, in this namespace, with the word "Entity".  Although one could 
reasonably argue that the namespace already conveys this,  it makes it easier to 
differentiate between otherwise identically-named "domain objects".  An example 
of this being difficult to read would be the automapper configuration.