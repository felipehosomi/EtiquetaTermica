﻿using System;

namespace CVA.AddOn.Common.Interfaces
{
    public interface IForm
    {
        Boolean AppEvent();

        Boolean FormDataEvent();

        void Freeze(Boolean freeze);

        Boolean ItemEvent();

        Boolean MenuEvent();

        Boolean PrintEvent();

        Boolean ProgressBarEvent();

        Boolean ReportDataEvent();

        Boolean RightClickEvent();

        Object Show();

        Object Show(string srfPath);

        Object Show(String[] args);

        Boolean StatusBarEvent();
    }
}