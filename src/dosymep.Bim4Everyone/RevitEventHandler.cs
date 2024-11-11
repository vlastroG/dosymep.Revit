﻿using System;
using System.Runtime.CompilerServices;

using Autodesk.Revit.UI;

namespace dosymep.Bim4Everyone {
    /// <summary>
    /// Класс для регистрации и обработки событий <see cref="Autodesk.Revit.UI.ExternalEvent"/>.
    /// Пример использования для вызова команд из не модальных окон:
    /// <code>
    /// // Сохраните ссылку на RevitEventHandler в приватное поле ViewModel вашего окна:
    /// private readonly RevitEventHandler;
    /// public ViewModel() {
    ///     _handler = new RevitEventHandler("RevitCommand");
    /// }
    /// 
    /// // Добавьте метод во ViewModel, который будет вызываться из не модального окна на клике кнопки.
    /// private async void DoCommand() {
    ///     await _handler.SetTransactAction(app => TaskDialog.Show(
    ///             "Handler sample",
    ///             $"Selected elements count: {app.ActiveUIDocument.Selection.GetElementIds().Count}"))
    ///         .Raise();
    /// }
    /// </code>
    /// </summary>
    public sealed class RevitEventHandler : IExternalEventHandler, INotifyCompletion {
        private readonly ExternalEvent _externalEvent;
        private readonly string _name;
        private Action _continuation;

        /// <summary>
        /// Создает экземпляр <see cref="Autodesk.Revit.UI.ExternalEvent"/> с обработчиком в качестве текущего экземпляра <see cref="RevitEventHandler"/>.
        /// </summary>
        public RevitEventHandler(string externalEventName) {
            if(externalEventName is null) {
                throw new ArgumentNullException(nameof(externalEventName));
            }
            if(string.IsNullOrWhiteSpace(externalEventName)) {
                throw new ArgumentException(nameof(externalEventName));
            }

            _externalEvent = ExternalEvent.Create(this);
            _name = externalEventName;
        }


        /// <summary>
        /// Флаг, показывающий, завершилось ли выполнение <see cref="TransactAction"/>, или нет.
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Делегат, который вызывается при обработке события <see cref="Autodesk.Revit.UI.ExternalEvent"/>.
        /// </summary>
        public Action<UIApplication> TransactAction { get; private set; }


        /// <summary>
        /// Назначает делегат <see cref="TransactAction"/>.
        /// </summary>
        /// <param name="action">Делегат, который будет вызван при обработке события <see cref="Autodesk.Revit.UI.ExternalEvent"/>.</param>
        /// <returns>Возвращает текущий объект <see cref="RevitEventHandler"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Исключение, если обязательный параметр null.</exception>
        public RevitEventHandler SetTransactAction(Action<UIApplication> action) {
            TransactAction = action ?? throw new ArgumentNullException(nameof(action));
            return this;
        }

        /// <inheritdoc/>
        public void Execute(UIApplication app) {
            try {
                TransactAction?.Invoke(app);
            } finally {
                IsCompleted = true;
                _continuation?.Invoke();
            }
        }

        /// <inheritdoc/>
        public string GetName() {
            return _name;
        }

        /// <summary>
        /// Вызывает событие <see cref="Autodesk.Revit.UI.ExternalEvent"/>, 
        /// экземпляр которого был создан при создании текущего экземпляра <see cref="RevitEventHandler"/>.
        /// </summary>
        /// <returns>Возвращает текущий объект <see cref="RevitEventHandler"/>.</returns>
        public RevitEventHandler Raise() {
            IsCompleted = false;
            _continuation = null;
            _externalEvent.Raise();
            return this;
        }

        /// <inheritdoc/>
        public void OnCompleted(Action continuation) {
            _continuation = continuation;
        }

        /// <summary>
        /// Возвращает awaiter объект.
        /// </summary>
        /// <returns>Возвращает текущий объект.</returns>
        public RevitEventHandler GetAwaiter() {
            return this;
        }

        /// <summary>
        /// Заканчивает ожидание завершения асинхронной задачи.
        /// </summary>
        public void GetResult() { }
    }
}
