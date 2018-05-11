using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sample.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<BindableObject>
    {
        private Delegate _eventHandler;
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create (nameof (EventName), typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create (nameof (Command), typeof(ICommand), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create (nameof (CommandParameter), typeof(object), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create (nameof (Converter), typeof(IValueConverter), typeof(EventToCommandBehavior), null);

        public string EventName 
        {
            get { return (string)GetValue (EventNameProperty); }
            set { SetValue (EventNameProperty, value); }
        }

        public ICommand Command 
        {
            get { return (ICommand)GetValue (CommandProperty); }
            set { SetValue (CommandProperty, value); }
        }

        public object CommandParameter 
        {
            get { return GetValue (CommandParameterProperty); }
            set { SetValue (CommandParameterProperty, value); }
        }

        public IValueConverter Converter 
        {
            get { return (IValueConverter)GetValue (InputConverterProperty); }
            set { SetValue (InputConverterProperty, value); }
        }

        protected override void OnAttachedTo (BindableObject bindable)
        {
            base.OnAttachedTo (bindable);
            RegisterEvent (EventName);
        }

        protected override void OnDetachingFrom (BindableObject bindable)
        {
            DeregisterEvent (EventName);
            base.OnDetachingFrom (bindable);
        }

        private void RegisterEvent (string name)
        {
            if (!string.IsNullOrEmpty (name))
            {
                var eventInfo = AssociatedObject.GetType ().GetRuntimeEvent (name);
                if (eventInfo == null) 
                {
                    throw new ArgumentException (string.Format ("EventToCommandBehavior: Can't register the '{0}' event.", EventName));
                }
                var methodInfo = typeof(EventToCommandBehavior).GetTypeInfo ().GetDeclaredMethod ("OnEvent");
                _eventHandler = methodInfo.CreateDelegate (eventInfo.EventHandlerType, this);
                eventInfo.AddEventHandler (AssociatedObject, _eventHandler);
            }
        }

        private void DeregisterEvent (string name)
        {
            if (!string.IsNullOrEmpty(name) && _eventHandler != null) 
            {
                var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
                if (eventInfo == null)
                {
                    throw new ArgumentException (string.Format ("EventToCommandBehavior: Can't de-register the '{0}' event.", EventName));
                }
                eventInfo.RemoveEventHandler (AssociatedObject, _eventHandler);
                _eventHandler = null;
            }
        }

        private void OnEvent (object sender, object eventArgs)
        {
            if (Command != null) 
            {
                var resolvedParameter = GetCommandParameter (eventArgs);
                if (Command.CanExecute (resolvedParameter)) 
                {
                    Command.Execute (resolvedParameter);
                }
            }
        }

        private object GetCommandParameter(object eventArgs)
        {
            if (CommandParameter != null) 
            {
                return CommandParameter;
            }
            return Converter != null ? Converter.Convert (eventArgs, typeof(object), null, null) : eventArgs;
        }

        private static void OnEventNameChanged (BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = bindable as EventToCommandBehavior;
            if (behavior?.AssociatedObject != null) 
            {
                behavior.DeregisterEvent (oldValue as string);
                behavior.RegisterEvent (newValue as string);
            }
        }
    }
}
