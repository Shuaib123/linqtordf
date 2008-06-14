[OwlResource(OntologyName = "$handle$", RelativeUriReference = "$it.Uri$")]
public string $it.Range$Uri { get; set; }

private EntityRef<$it.Range$> _$it.Range$ { get; set; }

[OwlResource(OntologyName = "$handle$", RelativeUriReference = "$it.Uri$")]
public $it.Range$ $it.Range$
{
    get
    {
        if (_$it.Range$.HasLoadedOrAssignedValue)
            return _$it.Range$.Entity;
        if (DataContext != null)
        {
            var ctx = ($handle$DataContext)DataContext;
            _$it.Range$ = new EntityRef<$it.Range$>(from x in ctx.$it.Range$s where x.HasInstanceUri($it.Range$Uri) select x);
            return _$it.Range$.Entity;
        }
        return null;
    }
}
