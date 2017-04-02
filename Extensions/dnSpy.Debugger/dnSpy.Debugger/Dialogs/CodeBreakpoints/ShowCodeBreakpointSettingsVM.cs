﻿/*
    Copyright (C) 2014-2017 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using dnSpy.Contracts.Debugger.Breakpoints.Code;
using dnSpy.Contracts.MVVM;
using dnSpy.Debugger.Properties;

namespace dnSpy.Debugger.Dialogs.CodeBreakpoints {
	sealed class ShowCodeBreakpointSettingsVM : ViewModelBase {
		public bool EnableConditionalExpression {
			get => enableConditionalExpression;
			set {
				if (enableConditionalExpression == value)
					return;
				enableConditionalExpression = value;
				OnPropertyChanged(nameof(EnableConditionalExpression));
			}
		}
		bool enableConditionalExpression;

		static readonly EnumVM[] conditionList = new EnumVM[] {
			new EnumVM(DbgCodeBreakpointConditionKind.IsTrue, dnSpy_Debugger_Resources.Breakpoint_Condition_IsTrue),
			new EnumVM(DbgCodeBreakpointConditionKind.WhenChanged, dnSpy_Debugger_Resources.Breakpoint_Condition_WhenChanged),
		};
		public EnumListVM ConditionalExpression_Items => conditionalExpression_Items;
		readonly EnumListVM conditionalExpression_Items = new EnumListVM(conditionList);

		public string ConditionalExpression_Text {
			get => conditionalExpression_Text;
			set {
				if (conditionalExpression_Text == value)
					return;
				conditionalExpression_Text = value;
				OnPropertyChanged(nameof(ConditionalExpression_Text));
			}
		}
		string conditionalExpression_Text;

		public bool EnableHitCount {
			get => enableHitCount;
			set {
				if (enableHitCount == value)
					return;
				enableHitCount = value;
				OnPropertyChanged(nameof(EnableHitCount));
				HasErrorUpdated();
			}
		}
		bool enableHitCount;

		static readonly EnumVM[] hitCountList = new EnumVM[] {
			new EnumVM(DbgCodeBreakpointHitCountKind.Equals, "="),
			new EnumVM(DbgCodeBreakpointHitCountKind.MultipleOf, dnSpy_Debugger_Resources.Breakpoint_HitCount_IsAMultipleOf),
			new EnumVM(DbgCodeBreakpointHitCountKind.GreaterThanOrEquals, ">="),
		};
		public EnumListVM HitCount_Items => hitCount_Items;
		readonly EnumListVM hitCount_Items = new EnumListVM(hitCountList);

		public Int32VM HitCount_Text { get; }

		public bool EnableFilter {
			get => enableFilter;
			set {
				if (enableFilter == value)
					return;
				enableFilter = value;
				OnPropertyChanged(nameof(EnableFilter));
			}
		}
		bool enableFilter;

		public string Filter_Text {
			get => filter_Text;
			set {
				if (filter_Text == value)
					return;
				filter_Text = value;
				OnPropertyChanged(nameof(Filter_Text));
			}
		}
		string filter_Text;

		public bool EnableTrace {
			get => enableTrace;
			set {
				if (enableTrace == value)
					return;
				enableTrace = value;
				OnPropertyChanged(nameof(EnableTrace));
			}
		}
		bool enableTrace;

		public string Trace_Text {
			get => trace_Text;
			set {
				if (trace_Text == value)
					return;
				trace_Text = value;
				OnPropertyChanged(nameof(Trace_Text));
			}
		}
		string trace_Text;

		public bool Trace_Continue {
			get => trace_Continue;
			set {
				if (trace_Continue == value)
					return;
				trace_Continue = value;
				OnPropertyChanged(nameof(Trace_Continue));
			}
		}
		bool trace_Continue;

		public bool IsEnabled {
			get => isEnabled;
			set {
				if (isEnabled == value)
					return;
				isEnabled = value;
				OnPropertyChanged(nameof(IsEnabled));
			}
		}
		bool isEnabled;

		public ShowCodeBreakpointSettingsVM(DbgCodeBreakpointSettings settings) {
			HitCount_Text = new Int32VM(a => HasErrorUpdated()) { UseDecimal = true };
			Initialize(settings);
		}

		void Initialize(DbgCodeBreakpointSettings settings) {
			IsEnabled = settings.IsEnabled;
			Initialize(settings.Condition);
			Initialize(settings.HitCount);
			Initialize(settings.Filter);
			Initialize(settings.Trace);
		}

		void Initialize(DbgCodeBreakpointCondition? condition) {
			if (condition == null) {
				EnableConditionalExpression = false;
				ConditionalExpression_Items.SelectedIndex = 0;
				ConditionalExpression_Text = string.Empty;
			}
			else {
				EnableConditionalExpression = true;
				ConditionalExpression_Items.SelectedIndex = ConditionalExpression_Items.GetIndex(condition.Value.Kind);
				ConditionalExpression_Text = condition.Value.Condition ?? string.Empty;
			}
		}

		void Initialize(DbgCodeBreakpointHitCount? hitCount) {
			if (hitCount == null) {
				EnableHitCount = false;
				HitCount_Items.SelectedIndex = 0;
				HitCount_Text.Value = 0;
			}
			else {
				EnableHitCount = true;
				HitCount_Items.SelectedIndex = HitCount_Items.GetIndex(hitCount.Value.Kind);
				HitCount_Text.Value = hitCount.Value.Count;
			}
		}

		void Initialize(DbgCodeBreakpointFilter? filter) {
			if (filter == null) {
				EnableFilter = false;
				Filter_Text = string.Empty;
			}
			else {
				EnableFilter = true;
				Filter_Text = filter.Value.Filter ?? string.Empty;
			}
		}

		void Initialize(DbgCodeBreakpointTrace? trace) {
			if (trace == null) {
				EnableTrace = false;
				Trace_Text = string.Empty;
				Trace_Continue = true;
			}
			else {
				EnableTrace = true;
				Trace_Text = trace.Value.Message ?? string.Empty;
				Trace_Continue = trace.Value.Continue;
			}
		}

		public DbgCodeBreakpointSettings GetSettings() => new DbgCodeBreakpointSettings {
			IsEnabled = IsEnabled,
			Condition = GetCondition(),
			HitCount = GetHitCount(),
			Filter = GetFilter(),
			Trace = GetTrace(),
		};

		DbgCodeBreakpointCondition? GetCondition() {
			if (!EnableConditionalExpression)
				return null;
			return new DbgCodeBreakpointCondition((DbgCodeBreakpointConditionKind)ConditionalExpression_Items.SelectedItem, ConditionalExpression_Text ?? string.Empty);
		}

		DbgCodeBreakpointHitCount? GetHitCount() {
			if (!EnableHitCount)
				return null;
			return new DbgCodeBreakpointHitCount((DbgCodeBreakpointHitCountKind)HitCount_Items.SelectedItem, HitCount_Text.Value);
		}

		DbgCodeBreakpointFilter? GetFilter() {
			if (!EnableFilter)
				return null;
			return new DbgCodeBreakpointFilter(Filter_Text ?? string.Empty);
		}

		DbgCodeBreakpointTrace? GetTrace() {
			if (!EnableTrace)
				return null;
			return new DbgCodeBreakpointTrace(Trace_Text ?? string.Empty, Trace_Continue);
		}

		public override bool HasError => EnableHitCount && HitCount_Text.HasError;
	}
}
