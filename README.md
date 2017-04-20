# net-eventbus
Event Bus for .NET

Register: EventBus.Register(this);

Subscribe: [Subscribe] private void OnEvent(string value);<br/>
Subscribe with name: [Subscribe Name="Event"] private void OnEvent(string value);<br/>
Subscribe on background thread: [Subscribe ThreadMode=ThreadMode.Background] private void OnEvent(string value);<br/>

Fire: EventBus.Fire(value); or EventBus.Fire("event", value);
