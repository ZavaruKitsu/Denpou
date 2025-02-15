﻿using System;
using System.Collections.Generic;
using Denpou.Controls.Hybrid;
using Denpou.Form;
using Denpou.Interfaces;

namespace Denpou.DataSources;

public class ButtonFormDataSource : IDataSource<ButtonRow>
{
    private ButtonForm _buttonForm;

    public ButtonFormDataSource()
    {
        _buttonForm = new ButtonForm();
    }

    public ButtonFormDataSource(ButtonForm bf)
    {
        _buttonForm = bf;
    }

    public virtual ButtonForm ButtonForm
    {
        get => _buttonForm;
        set => _buttonForm = value;
    }


    /// <summary>
    ///     Returns the amount of rows.
    /// </summary>
    public virtual int RowCount => ButtonForm.Rows;

    /// <summary>
    ///     Returns the maximum amount of columns.
    /// </summary>
    public virtual int ColumnCount => ButtonForm.Cols;


    /// <summary>
    ///     Returns the amount of rows existing.
    /// </summary>
    /// <returns></returns>
    public virtual int Count => ButtonForm.Count;

    /// <summary>
    ///     Returns the row with the specific index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public virtual ButtonRow ItemAt(int index)
    {
        return ButtonForm[index];
    }

    public virtual List<ButtonRow> ItemRange(int start, int count)
    {
        return ButtonForm.GetRange(start, count);
    }

    public virtual List<ButtonRow> AllItems()
    {
        return ButtonForm.ToArray();
    }

    public virtual ButtonForm PickItems(int start, int count, string filter = null)
    {
        var bf = new ButtonForm();
        ButtonForm dataForm = null;

        if (filter == null)
            dataForm = ButtonForm.Duplicate();
        else
            dataForm = ButtonForm.FilterDuplicate(filter, true);

        for (var i = 0; i < count; i++)
        {
            var it = start + i;

            if (it > dataForm.Rows - 1)
                break;

            bf.AddButtonRow(dataForm[it]);
        }

        return bf;
    }

    public virtual ButtonForm PickAllItems(string filter = null)
    {
        if (filter == null)
            return ButtonForm.Duplicate();


        return ButtonForm.FilterDuplicate(filter, true);
    }

    public virtual Tuple<ButtonRow, int> FindRow(string text, bool useText = true)
    {
        return ButtonForm.FindRow(text, useText);
    }

    /// <summary>
    ///     Returns the maximum items of this data source.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public virtual int CalculateMax(string filter = null)
    {
        return PickAllItems(filter).Rows;
    }

    public virtual ButtonRow Render(object data)
    {
        return data as ButtonRow;
    }


    public static implicit operator ButtonFormDataSource(ButtonForm bf)
    {
        return new ButtonFormDataSource(bf);
    }

    public static implicit operator ButtonForm(ButtonFormDataSource ds)
    {
        return ds.ButtonForm;
    }
}