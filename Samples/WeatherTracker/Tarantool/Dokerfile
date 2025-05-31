FROM tarantool/tarantool:3.4.0

COPY sample_app.lua /opt/tarantool

EXPOSE 3301
CMD ["tarantool", "/opt/tarantool/sample_app.lua"]