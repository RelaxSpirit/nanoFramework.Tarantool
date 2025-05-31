box.cfg{listen = 3301}

local function bootstrap()
    
    box.schema.user.grant('guest', 'read,write,execute', 'universe')

box.schema.space.create('weatherTracker')

box.schema.sequence.create('id_seq')
box.sequence.id_seq:next()

box.space.weatherTracker:format({ 
    { name = 'id', type = 'unsigned' }, 
    { name = 'device_mac_address', type = 'string' }, 
    { name = 'date_time', type = 'unsigned' }, 
    { name = 'temperature', type = 'double' }, 
    { name = 'pressure', type = 'double' }})

box.space.weatherTracker:create_index('primary',{ sequence = 'id_seq' })
box.space.weatherTracker:create_index('device_date', { parts = { { 'device_mac_address' }, { 'date_time' } } })
box.space.weatherTracker:create_index('temperature', { parts = { { 'temperature' } }, unique = false })
box.space.weatherTracker:create_index('pressure', { parts = { { 'pressure' } }, unique = false })

end

box.once('startup', bootstrap)